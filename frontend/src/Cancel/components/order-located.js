import React from "react";
import Divider from "@material-ui/core/Divider";
import Typography from "@material-ui/core/Typography";
import Container from "@material-ui/core/Container";

import { makeStyles } from "@material-ui/core/styles";
import Grid from "@material-ui/core/Grid";
import TextField from '@material-ui/core/TextField';

const useStyles = makeStyles(theme => ({
  root: {
    flexGrow: 1
  },
  paper: {
    padding: theme.spacing(2),
    textAlign: "center",
    color: theme.palette.text.secondary,
    minHeight: 150
  },
  rootForm: {
    '& > *': {
      margin: theme.spacing(1),
      display: "flex",
      flex:1
    }   
    }
}));

export default function SimpleContainer(props) {
  const classes = useStyles();
  return (
    <React.Fragment>
      <Container style={{ backgroundColor: "white", marginTop: 20 }}>
        <Typography variant="h4" gutterBottom>
          ¿Confirma anular orden 9000 a nombre de Eddui Mendez por un monto de $15,000?
        </Typography>
        <Divider />
        <div className={classes.root}>
      <Grid container spacing={3}>
        <Grid item xs={12}>
          {/* <Paper className={classes.paper}>xs=12</Paper> */}
        </Grid>
        

        <Grid item xs={12} sm={4}>
          
        </Grid>
      </Grid>
    </div>
        
      </Container>
    </React.Fragment>
  );
}
