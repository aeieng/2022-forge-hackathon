import { Button, Form, Input, InputNumber, Layout, Slider, Space } from "antd";
import { useEffect, useState } from "react";
import { MinusCircleOutlined, PlusOutlined } from "@ant-design/icons";
import { v4 as uuidv4 } from 'uuid';

const BuildingProgram:React.FC<{buildingId: string}> = ({buildingId}) => {
  const [loading, setLoading] = useState(true);
  const [buildingInfo, setBuildingInfo] = useState({})

  useEffect(() => {
    setLoading(true)
    fetch(`https://localhost:5001/building-program?buildingId=${buildingId}`)
      .then((response) => response.json())
      .then((data) => setBuildingInfo({roomTypes: data}))
      .finally(() => setLoading(false));
  }, [buildingId]);

  const onFinish = (values: any) => {
    fetch(`https://localhost:5001/building-program?buildingId=${buildingId}`, {
      method: 'POST',
      headers: new Headers({
        "Content-Type": "application/json",
      }),
      body: JSON.stringify(values.roomTypes)
    })
  };

  if(loading) return <>loading...</>

  return (
    <Layout.Content>
      <Layout style={{ background: "#fff", padding: "10px" }}>
        <Form
          name="Program"
          onFinish={onFinish}
          autoComplete="off"
          layout="horizontal"
          initialValues={buildingInfo}
        >
          <Form.List name="roomTypes">
            {(fields, { add, remove }) => (
              <>
                {fields.map(({ key, name, ...restField }) => (
                  <Space key={key} size={[8, 16]} wrap>
                    <Form.Item
                      label="Room Type"
                      {...restField}
                      name={[name, "roomTypeName"]}
                      rules={[
                        { required: true, message: "Missing room type name" },
                      ]}
                    >
                      <Input placeholder="Room Type Name" />
                    </Form.Item>
                    <Form.Item
                      label="Percentage"
                      style={{ width: 150 }}
                      {...restField}
                      name={[name, "percentage"]}
                      rules={[
                        { required: true, message: "Missing percentage" },
                      ]}
                    >
                      <Slider min={0} max={100} />
                    </Form.Item>
                    <Form.Item
                      label="PeopleDensity"
                      {...restField}
                      name={[name, "peopleDensity"]}
                      rules={[
                        { required: true, message: "Missing people density" },
                      ]}
                    >
                      <InputNumber />
                    </Form.Item>
                    <Form.Item
                      label="Lighting Density"
                      {...restField}
                      name={[name, "lightingDensity"]}
                      rules={[
                        { required: true, message: "Missing lighting density" },
                      ]}
                    >
                      <InputNumber />
                    </Form.Item>
                    <Form.Item
                      label="Equipment Density"
                      {...restField}
                      name={[name, "equipmentDensity"]}
                      rules={[
                        {
                          required: true,
                          message: "Missing equipment density",
                        },
                      ]}
                    >
                      <InputNumber />
                    </Form.Item>
                    <MinusCircleOutlined onClick={() => remove(name)} />
                  </Space>
                ))}
                <Form.Item>
                  <Button
                    type="dashed"
                    onClick={() => add({ id: uuidv4(), peopleDensity: 25, lightingDensity: 1, equipmentDensity: 3})}
                    block
                    icon={<PlusOutlined />}
                  >
                    Add field
                  </Button>
                </Form.Item>
              </>
            )}
          </Form.List>
          <Form.Item>
            <Button type="primary" htmlType="submit">
              Submit
            </Button>
          </Form.Item>
        </Form>
      </Layout>
    </Layout.Content>
  );
};

export default BuildingProgram;
