import { useContext, useEffect, useState } from "react";
import { Modal, Select, Tree, TreeDataNode } from "antd";
import { ModelContext } from "../pages/Admin";
import { Model } from "./ModelTable";

const AUTODESK_API = "https://developer.api.autodesk.com";

type Hub = { id: string; attributes: { name: string } };

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
  const { setModels } = useContext(ModelContext);
  const [treeData, setTreeData] = useState<TreeDataNode[]>();
  const [token, setToken] = useState<Token>();
  const [modelToAdd, setModelToAdd] = useState<Model>();

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
                  title: o.attributes.name,
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
        onSelect={(selectedKeys, info) => {
          console.log(info);
          setModelToAdd({
            key: info.node.key,
            title: info.node.title,
          } as Model);
        }}
      />
      <Modal
        open={modelToAdd !== undefined}
        onCancel={() => setModelToAdd(undefined)}
        onOk={() => {
          if (
            modelToAdd !== undefined &&
            modelToAdd.discipline &&
            modelToAdd.buildingName
          ) {
            setModels((prev: Model[]) => [...prev, modelToAdd]);
            setModelToAdd(undefined);
          }
        }}
      >
        <Select
          options={[
            { label: "Mechanical", value: "mechanical" },
            { label: "Architectural", value: "architectural" },
            { label: "Electrical", value: "electrical" },
          ]}
          value={modelToAdd?.discipline}
          onSelect={(value: string) => {
            setModelToAdd((prev) =>
              prev !== undefined ? { ...prev, discipline: value } : prev
            );
          }}
          style={{ width: "250px" }}
        />
        <Select
          options={[{ label: "Building 1", value: "building1" }]}
          value={modelToAdd?.buildingName}
          onSelect={(value: string) => {
            setModelToAdd((prev) =>
              prev !== undefined ? { ...prev, buildingName: value } : prev
            );
          }}
          style={{ width: "250px" }}
        />
      </Modal>
    </>
  );
};

export default HubTree;
