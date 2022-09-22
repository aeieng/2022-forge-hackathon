import { Layout, Tabs } from "antd";

import BuildingGeneral from "./BuildingGeneral";
import BuildingProgram from "./BuildingProgram";
import BuildingEmbodiedCarbon from "./BuildingEmbodiedCarbon";

interface TabProps {
  buildingId: string;
  tabKey: string;
  setTabKey: React.Dispatch<React.SetStateAction<string>>;
}

export const BuildingTabs: React.FC<TabProps> = ({
  tabKey,
  setTabKey,
  buildingId,
}) => {
  const items = [
    {
      label: "General Information",
      key: "general-information",
      children: <BuildingGeneral buildingId={buildingId} />,
    },
    {
      label: "Program",
      key: "program",
      children: <BuildingProgram buildingId={buildingId} />,
    },
    {
      label: "Embodied Carbon",
      key: "embodied-carbon",
      children: <BuildingEmbodiedCarbon buildingId={buildingId} />,
    },
  ];

  return (
    <Layout.Content>
      <Layout style={{ background: "#fff", padding: "10px", height: "100%" }}>
        <Tabs
          activeKey={tabKey}
          onTabClick={(key) => setTabKey(key)}
          items={items}
        />
      </Layout>
    </Layout.Content>
  );
};
