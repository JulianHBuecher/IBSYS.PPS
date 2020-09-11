import React, { useCallback } from "react";
import { useDropzone } from "react-dropzone";
import styled from "styled-components";

export default function Home() {
  const onDrop = useCallback((acceptedFiles) => {
    // Do something with the files
  }, []);
  const { getRootProps, getInputProps, isDragActive } = useDropzone({ onDrop });

  const DragField = styled.div`
    background-color: #ebebeb;
    border: 1px dotted black;
    text-align: center;
    font-size: 25px;
    border-radius: 10px;
    width: 300px;
    margin: 50px auto;
    padding: 10px;
  `;

  const DragContainer = styled.div`
    text-align: center;
  `;

  return (
    <DragContainer>
      <h1 className="sys-home-margin">Simulationsdatei hochladen</h1>
      <DragField className="sys-home-margin" {...getRootProps()}>
        <input {...getInputProps()} />
        {isDragActive ? (
          <span>Drop the files here ...</span>
        ) : (
          <span>Datei hochladen</span>
        )}
      </DragField>
      <button type="button" className="btn btn-primary btn-lg sys-home-margin">
        Simulation starten
      </button>
    </DragContainer>
  );
}
