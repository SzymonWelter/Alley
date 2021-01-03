import { getAlleyUrl } from "./configuration.service";

const getServices = (setServices) => {
  const url = `${getAlleyUrl()}/microservices`;
  fetch(url)
    .then((res) => res.json())
    .then((res) => {
      setServices(res);
    });
};

export { getServices };
