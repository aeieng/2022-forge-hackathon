import { useContext } from "react";
import { Table } from "antd";
import { ModelContext } from "../pages/Admin";

export type Model = {
  key: string;
  title: string;
  discipline: string;
  buildingName: string;
};

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
  },
];

const ModelTable = () => {
  const { models } = useContext(ModelContext);
  return <Table dataSource={models} columns={COLUMNS} pagination={false} />;
};

export default ModelTable;
