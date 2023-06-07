import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import Fab from "@material-ui/core/Fab";
import AddIcon from "@material-ui/icons/Add";
import SaveIcon from "@material-ui/icons/Save";

export default function FabButton(props) {
  const useStyles = makeStyles((theme) => ({
    margin: {
      margin: theme.spacing(1),
    },
    extendedIcon: {
      marginRight: theme.spacing(1),
    },
    fab: {
      position: "fixed",
      bottom: theme.spacing(2),
      right: theme.spacing(2),
      backgroundColor: props.backgroundColor,
      color: "white",
      zIndex: 99,
    },
  }));

  const classes = useStyles();
  return (
    <Fab
      aria-label="add"
      className={classes.fab}
      onClick={() => props.onClick()}
    >
      {props.icon === "add" ? <AddIcon /> : <SaveIcon />}
    </Fab>
  );
}
