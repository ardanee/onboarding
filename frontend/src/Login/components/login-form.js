import React from "react";
import Avatar from "@material-ui/core/Avatar";
import Button from "@material-ui/core/Button";
import CssBaseline from "@material-ui/core/CssBaseline";
import FormControl from "@material-ui/core/FormControl";
import Input from "@material-ui/core/Input";
import InputLabel from "@material-ui/core/InputLabel";
import LockOutlinedIcon from "@material-ui/icons/LockOutlined";
import Paper from "@material-ui/core/Paper";
import Typography from "@material-ui/core/Typography";
import AppBar from "@material-ui/core/AppBar";
import Toolbar from "@material-ui/core/Toolbar";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { makeStyles } from "@material-ui/core";

const useStyles = makeStyles((theme) => ({
  main: {
    width: "auto",
    display: "block", // Fix IE 11 issue.
    marginLeft: theme.spacing.unit * 3,
    marginRight: theme.spacing.unit * 3,
    [theme.breakpoints.up(400 + theme.spacing.unit * 3 * 2)]: {
      width: 400,
      marginLeft: "auto",
      marginRight: "auto",
    },
  },
  paper: {
    marginTop: theme.spacing.unit * 8,
    display: "flex",
    flexDirection: "column",
    alignItems: "center",
    padding: `${theme.spacing.unit * 2}px ${theme.spacing.unit * 3}px ${
      theme.spacing.unit * 3
    }px`,
  },
  avatar: {
    margin: theme.spacing.unit,
    backgroundColor: "#2B3C4D",
  },
  form: {
    width: "100%", // Fix IE 11 issue.
    marginTop: theme.spacing.unit,
  },
  submit: {
    marginTop: theme.spacing.unit * 3,
  },
  logoImg: {
    width: 167,
  },
}));

function LoginComponent(props) {
  const classes = useStyles();

  /* if (props.isLoading) {
    return <Redirect to={"/home"} />;
  } */
  return (
    <>
      <AppBar position="static">
        <Toolbar style={{ backgroundColor: "#2B3C4D" }}>
          <img
            src="/image-repository/logo.png"
            className={classes.logoImg}
            alt="logo ilu"
          />
        </Toolbar>
      </AppBar>
      <main className={classes.main}>
        <CssBaseline />
        <Paper className={classes.paper}>
          <Avatar className={classes.avatar}>
            <LockOutlinedIcon />
          </Avatar>
          <Typography component="h1" variant="h5">
            Ingreso a la aplicación
          </Typography>
          <div>
            <FormControl margin="normal" required fullWidth>
              <InputLabel htmlFor="usuario">Usuario (CUI)</InputLabel>
              <Input
                id="usuario"
                name="usuario"
                autoComplete="usuario"
                autoFocus
                onChange={(event) => props.onChangeUser(event.target.value)}
              />
            </FormControl>
            <FormControl margin="normal" required fullWidth>
              <InputLabel htmlFor="password">Contraseña</InputLabel>
              <Input
                name="password"
                type="password"
                id="password"
                autoComplete="current-password"
                onChange={(event) => props.onChangePassword(event.target.value)}
              />
            </FormControl>
            <Button
              fullWidth
              variant="contained"
              style={{
                backgroundColor: "#69BD4B",
                color: "white",
                alignItems: "center",
                justifyContent: "center",
              }}
              onClick={props.onClickLogin}
            >
              {props.isLoading ? <span>Cargando..</span> : <span></span>}
              &nbsp;Iniciar sesión
            </Button>
          </div>
        </Paper>
      </main>
      <ToastContainer />
    </>
  );
}

export default LoginComponent;
