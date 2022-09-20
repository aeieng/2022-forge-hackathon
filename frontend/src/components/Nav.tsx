import { useLocation } from "react-router-dom";
import { Menu } from "antd";
import { useNavigate } from "react-router-dom";

const Nav = () => {
  const navigate = useNavigate();
  const { pathname } = useLocation();

  return (
    <Menu
      onClick={({ key }) => navigate(`/${key}`)}
      selectedKeys={[pathname.split("/")[1]]}
      mode="horizontal"
      items={[
        {
          label: "Admin",
          key: "admin",
        },
        {
          label: "Inputs",
          key: "inputs",
        },
        {
          label: "Dashboard",
          key: "dashboard",
        },
      ]}
    />
  );
};

export default Nav;
