import { Button, Image } from "antd";
import { useNavigate } from "react-router-dom";

const Login = () => {
  const navigate = useNavigate();

  return (
    <div style={{ textAlign: "center" }}>
      <Button onClick={() => navigate("/admin")}>Login</Button>
      <div>
        <Image width={200} src="/building_image.png" />
        <Image width={200} src="/ui_image.png" />
      </div>
      <h1>Unlock data in your models...</h1>
    </div>
  );
};

export default Login;
