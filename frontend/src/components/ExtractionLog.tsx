import { Table, TableColumnsType } from "antd";
import { useContext, useEffect, useState } from "react";
import { StopOutlined } from "@ant-design/icons";
import { AppContext } from "../context/AppContext";
import { ModelContext } from "../context/ModelContext";

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

function sleep(ms: number) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}

let interval: number;

const ExtractionLog = () => {
  const { token } = useContext(AppContext);
  const { run, setRun } = useContext(ModelContext);
  const [extractionLog, setExtractionLog] = useState<ExtractionLogType[]>([]);

  useEffect(() => {
    if (run !== undefined) {
      setExtractionLog((prev) => {
        const i = prev.length - 1;
        prev[i] = { ...prev[i], status: "Running" };
        return prev;
      });
      const f = async () => {
        await sleep(5000);
        setRun(undefined);
      };
      f();
    }
  }, [run]);

  useEffect(() => {
    // Polling for extraction-log.
    interval = setInterval(
      () =>
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
            const next = data.map((o: any) => ({ ...o, status: "Successful" }));
            setExtractionLog(next);
          })
          .catch((error) => {
            console.error(error);
          }),
      1000
    );
  }, []);

  const dataSource = extractionLog;
  if (run !== undefined) {
    const i = dataSource.length - 1;
    dataSource[i] = { ...dataSource[i], status: "Running" };
  }

  return (
    <Table
      dataSource={dataSource}
      columns={COLUMNS}
      pagination={false}
      size="small"
      rowKey="id"
    />
  );
};

export default ExtractionLog;
