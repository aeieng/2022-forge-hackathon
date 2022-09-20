import { Button, Image } from "antd";

const Login = () => {
  return (
    <div>
      <Button>Login</Button>
      <div>
        <Image width={200} src="/building_image.png" />
        <Image width={200} src="/ui_image.png" />
      </div>
      <h1>Unlock data in your models...</h1>
    </div>
  );
};

export default Login;
