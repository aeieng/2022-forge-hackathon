import { useParams } from "react-router-dom";
import { Tabs } from "antd";
import { useNavigate } from "react-router-dom";
import Dashboard from "./dashboard";
import Admin from "./admin";

const Nav = () => {
  const { tabId } = useParams();
  const navigate = useNavigate();

  return (
    <Tabs
      defaultActiveKey={tabId}
      onChange={(activeKey) => {
        navigate(`/${activeKey}`);
      }}
      items={[
        {
          label: "Admin",
          key: "admin",
          children: <Admin />,
        },
        {
          label: "Inputs",
          key: "inputs",
          children: "Inputs page",
        },
        {
          label: "Dashboard",
          key: "dashboard",
          children: <Dashboard />,
        },
      ]}
    />
  );
};

export default Nav;
