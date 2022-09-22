import { Table, TableColumnsType } from "antd";
import { useEffect, useState } from "react";
import { StopOutlined } from "@ant-design/icons";

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
  const [extractionLog, setExtractionLog] = useState<ExtractionLogType[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    fetch("https://localhost:5001/token")
      .then((response) => {
        if (response.ok) {
          return response.json();
        }
        throw response;
      })
      .then((token) => {
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
            console.log(data);
            setExtractionLog(data);
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
  }, []);

  return (
    <Table
      loading={loading}
      dataSource={extractionLog}
      columns={COLUMNS}
      pagination={false}
      size="small"
    />
  );
};

export default ExtractionLog;
