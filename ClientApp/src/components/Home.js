import React, { useRef, useState } from "react";
import styled from "styled-components";
import axios from "axios";
import { FilePond } from "react-filepond";

export default function Home() {
  const DragContainer = styled.div`
    text-align: center;
  `;

  const dropZone = useRef(null);

  const [results, setResults] = useState(undefined);

  return (
    <DragContainer>
      <h1 className="sys-home-margin">Simulationsdatei hochladen</h1>
      <FilePond
        ref={dropZone}
        allowMultiple={false}
        acceptedFileTypes={["text/xml"]}
        instantUpload={true}
        dropOnPage={true}
        maxParallelUploads={1}
        server={{
          timeout: 5000,
          fetch: null,
          revert: null,
          process: (fieldName, file, metadata, load, error, progress) => {
            console.log(dropZone.current);
            console.log(dropZone.current.getFile().file);
            const encodedXML = dropZone.current
              .getFile()
              .getFileEncodeBase64String();
            const decodedXML = atob(encodedXML);

            axios({
              url: "https://localhost:3000",
              method: "POST",
              headers: { "Content-Type": "application/xml" },
              data: decodedXML,
              onUploadProgress: (e) => {
                progress(e.lengthComputable, e.loaded, e.total);
              },
            })
              .then(function (response) {
                console.log(response);
                if (response.status >= 200 && response.status < 300) {
                  load(response);
                  setResults(response.data);
                } else {
                  error("Upload fehlgeschlagen");
                }
              })
              .catch(function (errorMessage) {
                error("Upload fehlgeschlagen");
                const response = errorMessage.response;
              });
          },
        }}
        Datei
        hochladen
      />
      <button type="button" className="btn btn-primary btn-lg sys-home-margin">
        Simulation starten
      </button>
    </DragContainer>
  );
}
