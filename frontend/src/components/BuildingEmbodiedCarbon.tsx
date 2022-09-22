import { useEffect, useState } from "react";
import { Button, Form, Input, InputNumber, Layout, Select, Space } from "antd";
import { DeleteFilled, PlusOutlined } from "@ant-design/icons";
import { v4 as uuidv4 } from "uuid";

const categories = [
  { label: "Enclosure", value: "Enclosure" },
  { label: "Interiors", value: "Interiors" },
  { label: "Mechanical", value: "Mechanical" },
  { label: "Electrical", value: "Electrical" },
  { label: "Site", value: "Site" },
  { label: "Structure", value: "Structure" },
];

const subcategories = [
  { label: "Basement Construction", value: "BasementConstruction" },
  { label: "Foundations", value: "Foundations" },
  { label: "Exterior Enclosure", value: "ExteriorEnclosure" },
  { label: "Super Structure", value: "SuperStructure" },
  { label: "Roofing", value: "Roofing" },
  { label: "Interior Construction", value: "InteriorConstruction" },
  { label: "Mechanical", value: "Mechanical" },
  { label: "Electrical", value: "Electrical" },
];

const BuildingEmbodiedCarbon: React.FC<{ buildingId: string }> = ({
  buildingId,
}) => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(true);
  const [buildingInfo, setBuildingInfo] = useState({});

  useEffect(() => {
    setLoading(true);
    fetch(`https://localhost:5001/building-materials?buildingId=${buildingId}`)
      .then((response) => response.json())
      .then((data) => setBuildingInfo({ buildingMaterials: data }))
      .finally(() => setLoading(false));
  }, [buildingId]);

  const onFinish = (values: any) => {
    console.log(values);
    fetch(
      `https://localhost:5001/building-materials?buildingId=${buildingId}`,
      {
        method: "POST",
        headers: new Headers({
          "Content-Type": "application/json",
        }),
        body: JSON.stringify(values.buildingMaterials),
      }
    );
  };

  if (loading) return <>loading...</>;

  return (
    <Layout.Content>
      <Layout style={{ background: "#fff", padding: "10px" }}>
        <Form
          form={form}
          name="Program"
          onFinish={onFinish}
          autoComplete="off"
          layout="horizontal"
          initialValues={buildingInfo}
        >
          <Form.List name="buildingMaterials">
            {(fields, { add, remove }) => (
              <>
                {fields.map(({ key, name, ...restField }) => (
                  <Space key={key} size={[4, 10]} wrap>
                    <Form.Item
                      noStyle
                      shouldUpdate={(prevValues, curValues) =>
                        prevValues.area !== curValues.area ||
                        prevValues.sights !== curValues.sights
                      }
                    >
                      {() => (
                        <Form.Item
                          {...restField}
                          label="Category"
                          name={[name, "category"]}
                          rules={[{ required: true, message: "Missing category" }]}
                        >
                          <Select
                            style={{ width: 130 }}
                          >
                            {categories.map(({value, label}) => (
                              <Select.Option key={value} value={value}>
                                {label}
                              </Select.Option>
                            ))}
                          </Select>
                        </Form.Item>
                      )}
                    </Form.Item>
                    <Form.Item
                      noStyle
                      shouldUpdate={(prevValues, curValues) =>
                        prevValues.area !== curValues.area ||
                        prevValues.sights !== curValues.sights
                      }
                    >
                      {() => (
                        <Form.Item
                          {...restField}
                          label="SubCategory"
                          name={[name, "subCategory"]}
                          rules={[{ required: true, message: "Missing subcategory" }]}
                        >
                          <Select
                            style={{ width: 130 }}
                          >
                            {subcategories.map(({value, label}) => (
                              <Select.Option key={value} value={value}>
                                {label}
                              </Select.Option>
                            ))}
                          </Select>
                        </Form.Item>
                      )}
                    </Form.Item>
                    <Form.Item
                      label="Name"
                      {...restField}
                      name={[name, "name"]}
                      rules={[{ required: true, message: "Missing name" }]}
                    >
                      <Input />
                    </Form.Item>
                    <Form.Item
                      label="Quantity"
                      {...restField}
                      name={[name, "quantity"]}
                      rules={[{ required: true, message: "Missing quantity" }]}
                    >
                      <InputNumber style={{width: 60}} />
                    </Form.Item>
                    <Form.Item
                      label="Unit"
                      {...restField}
                      name={[name, "unit"]}
                      rules={[{ required: true, message: "Missing unit" }]}
                    >
                      <Input style={{width: 80}}/>
                    </Form.Item>
                    <Form.Item
                      label="Baseline EPD"
                      {...restField}
                      name={[name, "baselineEpd"]}
                      rules={[
                        {
                          required: true,
                          message: "Missing Baseline EPD",
                        },
                      ]}
                    >
                      <InputNumber />
                    </Form.Item>
                    <Form.Item
                    
                      label="Achievable EPD"
                      {...restField}
                      name={[name, "achievableEpd"]}
                      rules={[
                        {
                          required: true,
                          message: "Missing Achievable EPD",
                        },
                      ]}
                    >
                      <InputNumber style={{width: 80}} />
                    </Form.Item>
                    <Form.Item
                      label="Realized EPD"
                      {...restField}
                      name={[name, "realizedEpd"]}
                      rules={[
                        {
                          required: true,
                          message: "Missing Realized EPD",
                        },
                      ]}
                    >
                      <InputNumber />
                    </Form.Item>
                    <DeleteFilled style={{color: "red"}} onClick={() => remove(name)} />
                  </Space>
                ))}
                <Form.Item>
                  <Button
                    type="dashed"
                    color="red"
                    onClick={() =>
                      add({
                        id: uuidv4(),
                      })
                    }
                    block
                    icon={<PlusOutlined />}
                  >
                    Add Material
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

export default BuildingEmbodiedCarbon;
