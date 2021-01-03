import React, { useState, useEffect } from "react";
import { getServices } from "../services/microservices.service";

export function Home() {
  const initialServicesState = [];
  const [services, setServices] = useState(initialServicesState);

  useEffect(() => {
    getServices(setServices);
    return () => {
      setServices(initialServicesState);
    };
  }, []);

  return (
    <div>
      <h1>Microservices</h1>
      <dl>
        {services &&
          services.map((s, si) => (
            <div key={si}>
              <dt>{s.name}</dt>
              {s.instances.map((i, ii) => (
                <dd key={ii}>{i.uri}</dd>
              ))}
            </div>
          ))}
      </dl>
    </div>
  );
}
