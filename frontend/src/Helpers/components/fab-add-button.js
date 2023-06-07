import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Fab from '@material-ui/core/Fab';
import AddIcon from '@material-ui/icons/Add';

const useStyles = makeStyles(theme => ({
  margin: {
    margin: theme.spacing(1),
  },
  extendedIcon: {
    marginRight: theme.spacing(1),
  },
  fab: {
    position: 'fixed',
    bottom: theme.spacing(2),
    right: theme.spacing(2),
    backgroundColor: "#69BD4B",
    color: "white"
  },
}));

export default function FabAddButton(props) {
  const classes = useStyles();

  return (
    <div>
      <div>
        <Fab aria-label="add" className={classes.fab} onClick={() => props.onClick()}>
        <AddIcon />
        </Fab>
      </div>
    </div>
  );
}
