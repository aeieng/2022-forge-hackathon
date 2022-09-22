import { useEffect, useState } from "react";
import { Button, InputNumber, Input, Form, Layout } from "antd";

const InputsGeneral:React.FC<{buildingId: string}> = ({buildingId}) => {
  const [buildingInfo, setBuildingInfo] = useState()
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    setLoading(true)
    fetch(`https://localhost:5001/building?buildingId=${buildingId}`)
      .then((response) => response.json())
      .then((data) => setBuildingInfo(data))
      .finally(() => setLoading(false));
  }, [buildingId]);

  const onFinish = (values: any) => {
    const data = {id: buildingId, ...values}
    fetch('https://localhost:5001/building', {
      method: 'PUT',
      headers: new Headers({
        "Content-Type": "application/json",
      }),
      body: JSON.stringify(data)
    })
  };
  
  if(loading) return <>loading...</>

  return (
    <Layout.Content>
      <Layout style={{ background: "#fff", padding: "10px" }}>
          <Form
            layout="horizontal"
            name="basic"
            size="middle"
            labelCol={{ span: 4 }}
            wrapperCol={{ span: 8 }}
            initialValues={buildingInfo}
            onFinish={onFinish}
            autoComplete="off"
          >
            <Form.Item
              label="Name"
              name="name"
              rules={[
                { required: true, message: "Please input building name!" },
              ]}
            >
              <Input />
            </Form.Item>

            <Form.Item label="Project Number" name="projectNumber">
              <Input />
            </Form.Item>

            <Form.Item label="Primary Building Type" name="primaryBuildingType">
              <Input />
            </Form.Item>

            <Form.Item label="Floor Area" name="floorArea">
              <InputNumber disabled/>
            </Form.Item>

            <Form.Item wrapperCol={{ offset: 8, span: 16 }}>
              <Button type="primary" htmlType="submit">
                Submit
              </Button>
            </Form.Item>
          </Form>
      </Layout>
    </Layout.Content>
  );
};

export default InputsGeneral;
