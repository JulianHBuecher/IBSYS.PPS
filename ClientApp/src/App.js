import React from "react";
import { Route } from "react-router";
import { Layout } from "./components/Layout";
import Home from "./components/Home";

import * as FilePond from 'filepond';
import FilePondPluginFileValidateType from 'filepond-plugin-file-validate-type';
import FilePondPluginFileEncode from 'filepond-plugin-file-encode';
import 'filepond/dist/filepond.min.css';

import "./custom.css";
import ProductionOverview from "./components/production-overview";

FilePond.registerPlugin(FilePondPluginFileValidateType);
FilePond.registerPlugin(FilePondPluginFileEncode);


export default function App(props) {
      return (<Layout>
        <Route exact path="/" component={Home} />
        <Route
          exact
          path="/production-overview"
          component={ProductionOverview}
        />
      </Layout>
    );
  }