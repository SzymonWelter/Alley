import React, { useState, useEffect } from "react";

export function Home() {
  const initialServicesState = [];
  const [services, setServices] = useState(initialServicesState);

  useEffect(() => {
    fetch(`http://localhost:8080/microservices`)
      .then((res) => res.json())
      .then((res) => {
        setServices(res);
      });

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
