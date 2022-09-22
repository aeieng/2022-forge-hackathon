import { useContext, useState } from "react";
import { Table, TableColumnsType } from "antd";
import { DeleteOutlined } from "@ant-design/icons";
import { ModelContext } from "../context/ModelContext";

export type Building = {
  id: string;
  name: string;
};

export type Model = {
  key: string;
  title: string;
  discipline: string;
  building: Building;
  autodeskItemId: string;
  autodeskProjectId: string;
  autodeskHubId: string;
};

const ModelTable = () => {
  const { models, setModels } = useContext(ModelContext);
  const [loading, setLoading] = useState(false);

  const COLUMNS: TableColumnsType<Model> = [
    {
      title: "Model",
      dataIndex: "title",
      key: "title",
    },
    {
      title: "Discipline",
      dataIndex: "discipline",
      key: "discipline",
    },
    {
      title: "Building",
      dataIndex: ["building", "name"],
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
        // <Button
        //   onClick={() => {
        //     setModelToAdd(record);
        //   }}
        // >
        //   Edit
        // </Button>
        <DeleteOutlined
          alt="Delete Model"
          onClick={() => {
            setLoading(true);
            fetch("https://localhost:5001/token")
              .then((response) => {
                if (response.ok) {
                  return response.json();
                }
                throw response;
              })
              .then((token) => {
                fetch(
                  `https://localhost:5001/models/{id}/?modelId=${record.key}`,
                  {
                    method: "DELETE",
                    headers: new Headers({
                      Authorization: `Bearer ${token.accessToken}`,
                    }),
                  }
                )
                  .then((response) => {
                    if (response.ok) {
                      setModels((prev) => [
                        ...prev.filter((m) => m.key !== record.key),
                      ]);
                      return response.json();
                    }
                    throw response;
                  })
                  .then((data) => {
                    console.log(data);
                  })
                  .catch((error) => {
                    console.error(error);
                    alert("Error deleting.");
                  });
              })
              .catch((error) => {
                console.error(error);
              })
              .finally(() => {
                setLoading(false);
              });
          }}
        />
      ),
    },
  ];

  return (
    <Table
      loading={loading}
      dataSource={models}
      columns={COLUMNS}
      pagination={false}
      size="small"
    />
  );
};

export default ModelTable;
