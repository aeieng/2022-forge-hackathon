import { useLocation } from "react-router-dom";
import { Menu } from "antd";
import { useNavigate } from "react-router-dom";
import { BarChartOutlined, DeploymentUnitOutlined, EditOutlined, HomeOutlined } from "@ant-design/icons";

const Nav = () => {
  const navigate = useNavigate();
  const { pathname } = useLocation();

  return (
    <Menu
      onClick={({ key }) => navigate(`/${key}`)}
      selectedKeys={[pathname.split("/")[1]]}
      mode="horizontal"
      theme="dark"
      items={[
        {
          label: "Home",
          key: "",
          icon: <HomeOutlined/>
        },
        {
          label: "Admin",
          key: "admin",
          icon: <DeploymentUnitOutlined/>
        },
        {
          label: "Inputs",
          key: "inputs",
          icon: <EditOutlined />
        },
        {
          label: "Dashboard",
          key: "dashboard",
          icon: <BarChartOutlined />
        },
      ]}
    />
  );
};

export default Nav;
