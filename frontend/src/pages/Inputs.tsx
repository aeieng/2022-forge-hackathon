import { useState, useEffect } from "react";
import { Select, Space } from "antd";
import { BuildingTabs } from "../components/BuildingTabs";

interface Building {
  id: string,
  name: string
}

const Inputs = () => {
  const [buildings, setBuildings] = useState<Building[]>([]);
  const [buildingsLoading, setBuildingsLoading] = useState(true);
  const [tabKey, setTabKey] = useState("general-information");
  const [buildingId, setBuildingId] = useState<string | null>(null);

  useEffect(() => {
    fetch("https://localhost:5001/buildings")
      .then((response) => response.json())
      .then((data) => setBuildings(data))
      .finally(() => setBuildingsLoading(false));
  }, []);

  return (
    <Space direction="vertical" size="middle" style={{ display: "flex" }}>
      <Select
        value={buildingId}
        loading={buildingsLoading}
        onChange={(_, option) => {
          setBuildingId(option.value);
        }}
        style={{ alignSelf: "end", width: "300px" }}
      >
        {buildings.map((b) => (
          <Select.Option key={b.id} value={b.id}>
            {b.name}
          </Select.Option>
        ))}
      </Select>
      {buildingId ? (
        <BuildingTabs tabKey={tabKey} setTabKey={setTabKey} buildingId={buildingId} />
      ) : (
        <>Select a Building </>
      )}
    </Space>
  );
};

export default Inputs;
