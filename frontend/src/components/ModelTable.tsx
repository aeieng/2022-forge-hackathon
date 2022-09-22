import { useContext, useState } from "react";
import { Table, TableColumnsType } from "antd";
import { DeleteOutlined, EyeOutlined } from "@ant-design/icons";
import { Model, ModelContext } from "../context/ModelContext";
import { AppContext } from "../context/AppContext";
import ForgeModelViewModal from "./ForgeModelViewModal";

const ModelTable = () => {
  const { token } = useContext(AppContext);
  const { models, setModels } = useContext(ModelContext);
  const [viewModel, setViewModel] = useState<Model>();
  const [loading, setLoading] = useState(false);

  const COLUMNS: TableColumnsType<Model> = [
    {
      title: "Model",
      dataIndex: "name",
      key: "name",
    },
    {
      title: "Discipline",
      dataIndex: "type",
      key: "type",
    },
    {
      title: "Building",
      dataIndex: "buildingName",
      key: "buildingName",
    },
    {
      title: "Revit Version",
      dataIndex: "revitVersion",
      key: "revitVersion",
    },
    {
      title: "Actions",
      dataIndex: "actions",
      key: "actions",
      align: "center",
      render: (_, record) => (
        <>
          {/* <Button onClick={() => setModelToAdd(record)}>Edit</Button> */}
          <EyeOutlined
            alt="Forge Viewer"
            onClick={() => {
              setViewModel(record);
            }}
          />
          <span style={{ padding: "0 2px" }} />
          <DeleteOutlined
            alt="Delete Model"
            onClick={() => {
              setLoading(true);
              fetch(`https://localhost:5001/models/${record.id}`, {
                method: "DELETE",
                headers: new Headers({
                  Authorization: `Bearer ${token.accessToken}`,
                }),
              })
                .then((response) => {
                  if (response.ok) {
                    setModels((prev) => [
                      ...prev.filter((m) => m.id !== record.id),
                    ]);
                    return;
                  }
                  throw response;
                })
                .catch((error) => {
                  console.error(error);
                  alert("Error deleting.");
                })
                .finally(() => {
                  setLoading(false);
                });
            }}
          />
        </>
      ),
    },
  ];

  return (
    <>
      <Table
        loading={loading}
        dataSource={models}
        columns={COLUMNS}
        pagination={false}
        size="small"
        rowKey="id"
      />
      {viewModel !== undefined && (
        <ForgeModelViewModal
          model={viewModel}
          onClose={() => setViewModel(undefined)}
        />
      )}
    </>
  );
};

export default ModelTable;
