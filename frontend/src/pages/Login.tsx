import { Image, Typography } from "antd";

const Login = () => {
  return (
    <div style={{ textAlign: "center" }}>
      <div>
        <Typography.Title>
          Welcome to the team Hacker Overflow app!
        </Typography.Title>
        <Image width={250} src="/ui_image.png" />
      </div>
      <br />
      <Typography.Title level={3}>Technologies Used</Typography.Title>
      <Typography.Title level={5}>
        Dotnet, React, Revit, PostgresSQL, Power BI, Forge APIs (ACC/BIM 360,
        Design Automation, Model Derivative, Data Management, Viewer)
      </Typography.Title>
      <Typography.Title level={3}>Goals</Typography.Title>
      <Typography.Title level={5}>
        - Learn capabilities of FORGE
      </Typography.Title>
      <Typography.Title level={5}>- Eliminate spreadsheets</Typography.Title>
      <Typography.Title level={5}>
        - Use ACC/BIM 360 to share files
      </Typography.Title>
      <Typography.Title level={5}>
        - Unlock data in models using Design Automation/Model Derivative
      </Typography.Title>
      <Typography.Title level={5}>
        - Connect data to dashboards
      </Typography.Title>
    </div>
  );
};

export default Login;
