import React from "react";

import Typography from '@material-ui/core/Typography';
import { makeStyles, useTheme } from "@material-ui/core/styles";
import { withRouter } from "react-router-dom";
import Grid from "@material-ui/core/Grid";

function Card(props) {
  const useStyles = makeStyles(theme => ({
    container: {
      flex: 1,
      height: 250,
      backgroundColor: "#E74C3C",
      justifyContent: "center",
      alignItems: "center",
      borderRadius: 5,
      boxShadow: "0px 1px 8px rgba(0, 0, 0, 0.25)",
      cursor: "pointer"
    },
    option: {
      display: "flex",
      flex: 1,
      height: 180,
      backgroundColor: props.backgroundColor,
      borderRadius: "5px 5px 0px 0px",
      paddingLeft: 20
    },
    titleClass:{
      color: "white"
    },
    titleDiv:{
      display: "flex",
      flexDirection: "row",
      alignItems: "center",
      justifyContent: "center"
    },
    iconDiv:{
      marginRight: 15
    },
    children:{
      margin: 10
    }
  }));
  const classes = useStyles();

  return (
    <Grid item xs={12} sm={4}>
    <div className={classes.container} onClick = {() => props.onClick()}>
      <div className={classes.option}>
        <div className={classes.titleDiv}>
          <div className={classes.iconDiv}>{props.iconComponent} </div>
          <Typography variant="h4" gutterBottom className={classes.titleClass}>
            {props.title}
          </Typography>
        </div>
      </div>
      <div className={classes.children}>
        {props.children}
      </div>
    </div>
    </Grid>
  );
}

export default Card;
