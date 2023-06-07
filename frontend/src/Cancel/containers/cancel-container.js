import React from "react";

import { makeStyles } from "@material-ui/core/styles";
import Layout from "../components/layout";


const useStyles = makeStyles(theme => ({
  root: {
    flexGrow: 1
  },
  paper: {
    padding: theme.spacing(2),
    textAlign: "center",
    color: theme.palette.text.secondary,
    minHeight: 150
  }
}));


export default function CancelContainer() {
  return (
    <React.Fragment>
      
          <Layout />
        
      
    </React.Fragment>
  );
}
