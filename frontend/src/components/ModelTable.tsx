import { useContext } from "react";
import { Button, Table } from "antd";
import { ModelContext } from "../pages/Admin";

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
  const { models } = useContext(ModelContext);

  const COLUMNS = [
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
      // align: "center",
      // render: (_, record) => (
      //   <Button
      //     onClick={() => {
      //       setEditBuilding(record);
      //     }}
      //   >
      //     Edit
      //   </Button>
      // ),
    },
  ];

  return (
    <Table
      dataSource={models}
      columns={COLUMNS}
      pagination={false}
      size="small"
    />
  );
};

export default ModelTable;
