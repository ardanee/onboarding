import React, { Component } from "react";
import { withRouter } from "react-router-dom";

import Table from "@material-ui/core/Table";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";
import TableContainer from "@material-ui/core/TableContainer";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import Paper from "@material-ui/core/Paper";
import Typography from "@material-ui/core/Typography";

import IconButton from "@material-ui/core/IconButton";
import DeleteIcon from "@material-ui/icons/Delete";
import EditIcon from "@material-ui/icons/Edit";

import Layout from "../../Layout/layout";
import FloatingButton from "../../Helpers/components/fab-add-button";
import FormDialog from "../components/modal-create-provider";

function createData(name, calories, fat, carbs, protein) {
  return { name, calories, fat, carbs, protein };
}

const rows = [
  createData("Frozen yoghurt", 159, 6.0, 24, 4.0),
  createData("Ice cream sandwich", 237, 9.0, 37, 4.3),
  createData("Eclair", 262, 16.0, 24, 6.0),
  createData("Cupcake", 305, 3.7, 67, 4.3),
  createData("Gingerbread", 356, 16.0, 49, 3.9),
  createData("Frozen yoghurt", 159, 6.0, 24, 4.0),
  createData("Ice cream sandwich", 237, 9.0, 37, 4.3),
  createData("Eclair", 262, 16.0, 24, 6.0),
  createData("Cupcake", 305, 3.7, 67, 4.3),
  createData("Gingerbread", 356, 16.0, 49, 3.9),
  createData("Frozen yoghurt", 159, 6.0, 24, 4.0),
  createData("Ice cream sandwich", 237, 9.0, 37, 4.3),
  createData("Eclair", 262, 16.0, 24, 6.0),
  createData("Cupcake", 305, 3.7, 67, 4.3),
  createData("Gingerbread", 356, 16.0, 49, 3.9),
];

class ProductList extends Component {
  state = {
    open: false,
  };
  onClickAdd = () => {
    this.setState({ open: true });
  };

  handleCloseModal = () => {
    this.setState({ open: false });
  };

  render() {
    return (
      <Layout title="Proveedores">
        <React.Fragment>
          <FloatingButton onClick={this.onClickAdd} />
          <FormDialog
            ref={(element) => (this.formDialog = element)}
            open={this.state.open}
            handleCloseModal={this.handleCloseModal}
          />
          <TableContainer component={Paper}>
            <Table aria-label="simple table">
              <TableHead>
                <TableRow>
                  <TableCell>Código</TableCell>
                  <TableCell>Nombre</TableCell>
                  <TableCell>Descripción</TableCell>
                  <TableCell>Cantidad de productos</TableCell>
                  <TableCell>Opciones</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {rows.map((row) => (
                  <TableRow key={row.name}>
                    <TableCell component="th" scope="row">
                      <Typography variant="body2" color="textSecondary">
                        1
                      </Typography>
                    </TableCell>
                    <TableCell>
                      <Typography gutterBottom variant="subtitle1">
                        Distribuidora AQ
                      </Typography>
                    </TableCell>
                    <TableCell>
                      <Typography
                        gutterBottom
                        variant="subtitle1"
                        color="textPrimary"
                      >
                        Descripción de la tuplaDescripción de la tupla
                      </Typography>
                    </TableCell>
                    <TableCell>
                      <Typography
                        gutterBottom
                        variant="subtitle1"
                        color="error"
                      >
                        10
                      </Typography>
                    </TableCell>
                    <TableCell>
                      <IconButton
                        color="primary"
                        aria-label="add to shopping cart"
                      >
                        <EditIcon />
                      </IconButton>
                      <IconButton
                        color="primary"
                        aria-label="add to shopping cart"
                      >
                        <DeleteIcon />
                      </IconButton>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </React.Fragment>
      </Layout>
    );
  }
}

export default withRouter(ProductList);
