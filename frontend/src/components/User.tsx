import { UserOutlined } from "@ant-design/icons";
import { Avatar, Button, Popover } from "antd";
import { useNavigate } from "react-router-dom";

const User = () => {
  const navigate = useNavigate();

  const UserInfo = () => {
    return <Button onClick={() => navigate("/")}>Logout</Button>;
  };

  return (
    <Popover content={<UserInfo />} title="User Profile">
      <Avatar icon={<UserOutlined />} />
    </Popover>
  );
};

export default User;
