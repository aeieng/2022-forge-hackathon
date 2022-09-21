import { useState } from "react";
import { Select } from "antd";

const MOCK_BENCHMARK_BUILDINGS = [
  {
    benchmarkProjectId: 'MOCK_BENCHMARK_PROJECT_ID_1',
    benchmarkBuildingId: 'MOCK_BENCHMARK_BUILDING_ID_1',
    name: 'BUILDING_1'
  },
  {
    benchmarkProjectId: 'MOCK_BENCHMARK_PROJECT_ID_1',
    benchmarkBuildingId: 'MOCK_BENCHMARK_BUILDING_ID_2',
    name: 'BUILDING_2'
  },
  {
    benchmarkProjectId: 'MOCK_BENCHMARK_PROJECT_ID_1',
    benchmarkBuildingId: 'MOCK_BENCHMARK_BUILDING_ID_3',
    name: 'BUILDING_3'
  },
  {
    benchmarkProjectId: 'MOCK_BENCHMARK_PROJECT_ID_1',
    benchmarkBuildingId: 'MOCK_BENCHMARK_BUILDING_ID_4',
    name: 'BUILDING_4'
  }
];


const Inputs = () => {

  const [buildingId, setBuildingId] = useState();

  return <>
  <Select
  value={buildingId}
  onChange={(_, option) => {
    //setBuildingId(option.key);
  }}
  style={{ alignSelf: 'end', width: '300px' }}
>
{MOCK_BENCHMARK_BUILDINGS.map((bb) => (
        <Select.Option key={bb.benchmarkBuildingId} value={bb.benchmarkBuildingId}>
          {bb.name}
        </Select.Option>
      ))}
</Select></>;
};

export default Inputs;
