import { useContext, useEffect, useState } from "react";
import { Tree, TreeDataNode } from "antd";
import AddModelModal from "./AddModelModal";
import { ModelContext } from "../context/ModelContext";
import { AppContext } from "../context/AppContext";

const AUTODESK_API = "https://developer.api.autodesk.com";

type Hub = {
  id: string;
  attributes: { name?: string; displayName?: string; hidden?: boolean };
};

const hiddenFolderFilterCallback = (o: Hub) => {
  const notIncludedFolders = ["submittal", "checklist", "dailylog", "issue"];
  return (
    !o.attributes.hidden &&
    !o.attributes.name?.startsWith(
      notIncludedFolders.filter((s) => o.attributes.name?.startsWith(s))[0]
    )
  );
};

// It's just a simple demo. You can use tree map to optimize update perf.
const updateTreeData = (
  list: TreeDataNode[],
  key: React.Key,
  children: TreeDataNode[]
): TreeDataNode[] =>
  list.map((node) => {
    if (node.key === key) {
      return {
        ...node,
        children,
      };
    }
    if (node.children) {
      return {
        ...node,
        children: updateTreeData(node.children, key, children),
      };
    }
    return node;
  });

const HubTree = () => {
  const { token } = useContext(AppContext);
  const { setModelToAdd } = useContext(ModelContext);
  const [treeData, setTreeData] = useState<TreeDataNode[]>();
  const [loading, setLoading] = useState(false);

  const options = {
    method: "GET",
    headers: new Headers({
      Authorization: `Bearer ${token.accessToken}`,
    }),
  };

  useEffect(() => {
    if (token.accessToken && !loading && !treeData) {
      setLoading(true);
      fetch(`${AUTODESK_API}/project/v1/hubs`, options)
        .then((response) => response.json())
        .then((data) => {
          setTreeData(
            data.data.map((hub: Hub) => ({
              title: hub.attributes.name,
              key: hub.id,
              selectable: false,
            }))
          );
        })
        .finally(() => setLoading(false));
    }
  }, [token]);

  const onLoadData = ({ key, children }: any) => {
    return new Promise<void>((resolve) => {
      if (children) {
        resolve();
        return;
      }
      const keys = key.split("/");
      if (keys.length === 1) {
        // User opening a Hub in tree. Get Projects for this Hub.
        fetch(`${AUTODESK_API}/project/v1/hubs/${key}/projects`, options)
          .then((response) => response.json())
          .then((data) => {
            setTreeData((prev) =>
              updateTreeData(
                prev ?? [],
                key,
                data.data.map((o: Hub) => ({
                  title: o.attributes.name,
                  key: `${key}/${o.id}`,
                  selectable: false,
                }))
              )
            );
            resolve();
          });
      } else if (keys.length === 2) {
        // User opening a Project in tree. Get Folders for this Project.
        fetch(
          `${AUTODESK_API}/project/v1/hubs/${keys[0]}/projects/${keys[1]}/topFolders`,
          options
        )
          .then((response) => response.json())
          .then((data) => {
            setTreeData((prev) =>
              updateTreeData(
                prev ?? [],
                key,
                data.data.filter(hiddenFolderFilterCallback).map((o: Hub) => ({
                  title: o.attributes.name,
                  key: `${key}/${o.id}`,
                  selectable: false,
                }))
              )
            );
            resolve();
          });
      } else if (keys.length === 3) {
        // User opening a Folder in tree. Get Items for this Folder.
        fetch(
          `${AUTODESK_API}/data/v1/projects/${keys[1]}/folders/${keys[2]}/contents`,
          options
        )
          .then((response) => response.json())
          .then((data) => {
            setTreeData((prev) =>
              updateTreeData(
                prev ?? [],
                key,
                data.data.map((o: Hub) => ({
                  title: o.attributes.displayName,
                  key: `${key}/${o.id}`,
                  isLeaf: true,
                }))
              )
            );
            resolve();
          });
      }
    });
  };

  return (
    <>
      <Tree
        loadData={onLoadData}
        treeData={treeData}
        height={500}
        onSelect={(_, info) => {
          const [autodeskHubId, autodeskProjectId, autodeskItemId, modelId] = (
            info.node.key as string
          ).split("/");

          setModelToAdd({
            id: modelId,
            name: (info.node.title as string) ?? "none",
            autodeskItemId,
            autodeskProjectId,
            autodeskHubId,
            revitVersion: "",
            buildingId: "",
            type: "",
          });
        }}
      />
      <AddModelModal />
    </>
  );
};

export default HubTree;
