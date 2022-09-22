import { Table, TableColumnsType } from "antd";
import { useContext, useEffect, useState } from "react";
import { StopOutlined } from "@ant-design/icons";
import { AppContext } from "../context/AppContext";

type ExtractionLogType = {
  id: string;
  startedRunAtUtc: string;
  modelIds: string[];
  status: string;
};

const COLUMNS: TableColumnsType<ExtractionLogType> = [
  { title: "Id", dataIndex: "id", key: "id" },
  {
    title: "Started Run At",
    dataIndex: "startedRunAtUtc",
    key: "startedRunAtUtc",
  },
  { title: "Status", dataIndex: "status", key: "status" },
  {
    title: "Actions",
    dataIndex: "actions",
    key: "actions",
    align: "center",
    render: () => <StopOutlined alt="Cancel" onClick={() => alert("todo")} />,
  },
];

const ExtractionLog = () => {
  const { token } = useContext(AppContext);
  const [extractionLog, setExtractionLog] = useState<ExtractionLogType[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    fetch(`https://localhost:5001/extraction-log`, {
      method: "GET",
      headers: new Headers({
        Authorization: `Bearer ${token.accessToken}`,
      }),
    })
      .then((response) => {
        if (response.ok) {
          return response.json();
        }
        throw response;
      })
      .then((data) => {
        setExtractionLog(data);
      })
      .catch((error) => {
        console.error(error);
        alert("Error deleting.");
      })
      .finally(() => {
        setLoading(false);
      });
  }, []);

  return (
    <Table
      loading={loading}
      dataSource={extractionLog}
      columns={COLUMNS}
      pagination={false}
      size="small"
      rowKey="id"
    />
  );
};

export default ExtractionLog;
