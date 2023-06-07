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
          Datos de la orden
        </Typography>
        <Divider />
        <div className={classes.root}>
      <Grid container spacing={3}>
        <Grid item xs={12}>
          {/* <Paper className={classes.paper}>xs=12</Paper> */}
        </Grid>
        <Grid item xs={12}>
            <form className={classes.rootForm} noValidate autoComplete="off">
                <Grid container spacing={1}>
                    <Grid item xs={12} sm={4}>
                        <Typography variant="h6" gutterBottom>
                            Buscar orden:
                        </Typography>
                    </Grid>
                    <Grid item xs={12} sm={8}>
                        <TextField
                            id="filled-secondary"
                            label="Escribe el número de orden"
                            variant="filled"
                            color="primary"
                            fullWidth
                        />
                    </Grid>

                </Grid>

            </form>
        </Grid>
        

        <Grid item xs={12} sm={4}>
          
        </Grid>
      </Grid>
    </div>
        
      </Container>
    </React.Fragment>
  );
}
