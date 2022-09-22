import { useContext, useEffect, useState } from "react";
import { Tree, TreeDataNode } from "antd";
import { Model } from "./ModelTable";
import AddModelModal from "./AddModelModal";
import { ModelContext } from "../context/ModelContext";

const AUTODESK_API = "https://developer.api.autodesk.com";

type Hub = { id: string; attributes: { name?: string; displayName?: string } };

type Token = {
  accessToken: string;
  expiresAt: string;
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
  const { setModelToAdd } = useContext(ModelContext);
  const [treeData, setTreeData] = useState<TreeDataNode[]>();
  const [token, setToken] = useState<Token>();

  // useEffect(() => {
  //   fetch("https://developer.api.autodesk.com/authentication/v1/authenticate", {
  //     method: "POST",
  //     mode: "cors",
  //     headers: new Headers({
  //       "content-type": "application/x-www-form-urlencoded",
  //       client_id: "CqRjmmTMt7TGXSOpPAuVuWGQYHPNwZXZ",
  //       client_secret: "VHBoZ9SNRdG6ZLh4",
  //       grant_type: "client_credentials",
  //       scope: "data:write data:read bucket:create bucket:delete",
  //       // "Access-Control-Allow-Origin": "http://127.0.0.1:3000",
  //     }),
  //     // body: JSON.stringify({
  //     // }),
  //   })
  //     .then((response) => response.json())
  //     .then((data) => console.log(data));
  // });

  useEffect(() => {
    if (!token) {
      fetch("https://localhost:5001/token")
        .then((response) => response.json())
        .then((data) => setToken(data));
    }
  });

  useEffect(() => {
    if (token && !treeData) {
      fetch(`${AUTODESK_API}/project/v1/hubs`, {
        method: "GET",
        headers: new Headers({
          Authorization: `Bearer ${token.accessToken}`,
        }),
      })
        .then((response) => response.json())
        .then((data) => {
          setTreeData(
            data.data.map((hub: Hub) => ({
              title: hub.attributes.name,
              key: hub.id,
              selectable: false,
            }))
          );
        });
    }
  }, [token]);

  const onLoadData = ({ key, children }: any) => {
    return new Promise<void>((resolve) => {
      if (children) {
        resolve();
        return;
      }
      const options = {
        method: "GET",
        headers: new Headers({
          Authorization: `Bearer ${token?.accessToken}`,
        }),
      };
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
                data.data.map((o: Hub) => ({
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
            key: modelId,
            title: info.node.title,
            autodeskItemId,
            autodeskProjectId,
            autodeskHubId,
          } as Model);
        }}
      />
      <AddModelModal />
    </>
  );
};

export default HubTree;
