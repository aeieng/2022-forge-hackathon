import {
  createContext,
  Dispatch,
  SetStateAction,
  useState,
  PropsWithChildren,
  useEffect,
  useContext,
} from "react";
import { AppContext } from "./AppContext";

export type Building = {
  id: string;
  name: string;
};

type ModelBase = {
  autodeskHubId: string;
  autodeskItemId: string;
  autodeskProjectId: string;
  buildingId: string;
  name: string;
  revitVersion: string;
  type: string;
  derivativeId: string;
};

export type ModelMutation = ModelBase;

export type ModelResponse = {
  id: string;
} & ModelBase;

export type Model = {
  buildingName?: string;
} & ModelResponse;

type Context = {
  models: Model[];
  setModels: Dispatch<SetStateAction<Model[]>>;
  modelToAdd: Model | undefined;
  setModelToAdd: Dispatch<SetStateAction<Model | undefined>>;
  derivatives: Map<string, string>;
  setDerivatives: Dispatch<SetStateAction<Map<string, string>>>;
  run: string | undefined;
  setRun: Dispatch<SetStateAction<string | undefined>>;
};

export const ModelContext = createContext<Context>({
  models: [],
  setModels: () => {},
  modelToAdd: undefined,
  setModelToAdd: () => {},
  derivatives: new Map(),
  setDerivatives: () => {},
  run: undefined,
  setRun: () => {},
});

export const ModelContextProvider = ({ children }: PropsWithChildren) => {
  const { token } = useContext(AppContext);
  const [models, setModels] = useState<Model[]>([]);
  const [modelToAdd, setModelToAdd] = useState<Model>();
  const [derivatives, setDerivatives] = useState<Map<string, string>>(
    new Map()
  );
  const [run, setRun] = useState<string>();

  useEffect(() => {
    fetch("https://localhost:5001/models", {
      method: "GET",
      headers: new Headers({
        Authorization: `Bearer ${token.accessToken}`,
        "Content-Type": "application/json",
      }),
    })
      .then((response) => {
        if (response.ok) {
          return response.json();
        }
        throw response;
      })
      .then((data: ModelResponse[]) => {
        setModels(
          data.map(
            (model: ModelResponse): Model => ({
              id: model.id,
              name: model.name,
              buildingId: model.buildingId,
              // Building name not provided, using id for now.
              buildingName: model.buildingId,
              type: model.type,
              autodeskItemId: model.autodeskItemId,
              autodeskProjectId: model.autodeskProjectId,
              autodeskHubId: model.autodeskHubId,
              revitVersion: "",
              derivativeId: model.derivativeId,
            })
          )
        );
      })
      .catch((error) => {
        console.error(error);
      });
  }, []);

  return (
    <ModelContext.Provider
      value={{
        models,
        setModels,
        modelToAdd,
        setModelToAdd,
        derivatives,
        setDerivatives,
        run,
        setRun,
      }}
    >
      {children}
    </ModelContext.Provider>
  );
};
