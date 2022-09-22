import { useEffect, useState } from "react";
import { Table, TableColumnsType } from "antd";

export type Activity = {
  id: string;
  name: string;
  modelType: number;
  appBundleId: number;
  activityType: number;
};

const COLUMNS: TableColumnsType<Activity> = [
  { title: "Activity Id", dataIndex: "id", key: "id" },
  { title: "Name", dataIndex: "name", key: "name" },
  { title: "Discipline", dataIndex: "modelType", key: "modelType" },
];

const ActivitiesTable = () => {
  const [activities, setActivities] = useState<Activity[]>([]);
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
        fetch(`https://localhost:5001/activities`, {
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
            setActivities(data);
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
      dataSource={activities}
      columns={COLUMNS}
      pagination={false}
      size="small"
    />
  );
};

export default ActivitiesTable;
