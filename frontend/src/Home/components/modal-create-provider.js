import React from "react";
import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogTitle from "@material-ui/core/DialogTitle";
import useMediaQuery from "@material-ui/core/useMediaQuery";
import { useTheme } from "@material-ui/core/styles";

export default function FormDialog(props) {
  const theme = useTheme();
  const fullScreen = useMediaQuery(theme.breakpoints.down("sm"));

  return (
    <div>
      <Dialog
        open={props.open}
        onClose={() => props.handleCloseModal()}
        aria-labelledby="form-dialog-title"
        fullScreen={fullScreen}
      >
        <DialogTitle id="form-dialog-title">Crear</DialogTitle>
        <DialogContent>
          <TextField
            id="outlined-full-width"
            label="Nombre del producto"
            fullWidth
            placeholder="ej. Reloj digital"
            margin="normal"
            InputLabelProps={{
              shrink: true,
            }}
            variant="outlined"
          />
          <TextField
            id="outlined-full-width"
            label="Descripción"
            fullWidth
            placeholder="ej. Descripción del reloj"
            margin="normal"
            InputLabelProps={{
              shrink: true,
            }}
            variant="outlined"
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={() => props.handleCloseModal()} color="primary">
            Cancelar
          </Button>
          <Button onClick={() => props.handleCloseModal()} color="primary">
            Crear
          </Button>
        </DialogActions>
      </Dialog>
    </div>
  );
}
