import React from "react";
import PropTypes from "prop-types";
import AppBar from "@material-ui/core/AppBar";
import CssBaseline from "@material-ui/core/CssBaseline";
import Divider from "@material-ui/core/Divider";
import Drawer from "@material-ui/core/Drawer";
import Hidden from "@material-ui/core/Hidden";
import IconButton from "@material-ui/core/IconButton";
import BarChartIcon from "@material-ui/icons/BarChart";
import ExitToAppIcon from "@material-ui/icons/ExitToApp";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import ListItemIcon from "@material-ui/core/ListItemIcon";
import ListItemText from "@material-ui/core/ListItemText";
import MenuIcon from "@material-ui/icons/Menu";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import { makeStyles, useTheme } from "@material-ui/core/styles";
import { withRouter } from "react-router-dom";

const drawerWidth = 240;

function Layout(props) {
  const { container } = props;
  const useStyles = makeStyles((theme) => {
    let styles = {
      root: {
        display: "flex",
      },
      drawer: {
        [theme.breakpoints.up("sm")]: {
          width: drawerWidth,
          flexShrink: 0,
        },
      },
      appBar: {
        [theme.breakpoints.up("sm")]: {
          width: `calc(100% - ${drawerWidth}px)`,
          marginLeft: drawerWidth,
          backgroundColor: "white",
          color: "#424242",
        },
      },
      menuButton: {
        marginRight: theme.spacing(2),
        [theme.breakpoints.up("sm")]: {
          display: "none",
        },
      },
      drawerStyle: { backgroundColor: "#2b3c4d" },
      drawerOption: { color: "white" },
      toolbar: {
        ...theme.mixins.toolbar,
        backgroundColor: "#202e3c",
        backgroundImage: "url(/static/logo.png)",
        backgroundRepeat: "no-repeat",
        backgroundSize: 180,
        backgroundPosition: "center",
      },
      toolbarBody: theme.mixins.toolbar,
      drawerPaper: {
        width: drawerWidth,
        backgroundColor: "#2b3c4d",
      },
      content: {
        flexGrow: 1,
        padding: theme.spacing(3),
      },
    };
    if (props.style) {
      styles = { ...styles, ...props.style };
    }
    return styles;
  });
  let classes = useStyles();
  const theme = useTheme();
  const [mobileOpen, setMobileOpen] = React.useState(false);
  //const [open, setOpen] = React.useState(false);

  const handleDrawerToggle = () => {
    setMobileOpen(!mobileOpen);
  };

  // const handleDrawerOpen = () => {
  //   setOpen(true);
  // };

  // const handleDrawerClose = () => {
  //   setOpen(false);
  // };

  const drawer = (
    <div className={classes.drawerStyle}>
      <div className={classes.toolbar} />
      <Divider />
      <List>
        <ListItem
          button
          onClick={() => {
            props.history.push("/home");
          }}
        >
          <ListItemIcon>
            <BarChartIcon className={classes.drawerOption} />
          </ListItemIcon>
          <ListItemText className={classes.drawerOption} primary={"HOME"} />
        </ListItem>

        <ListItem
          button
          onClick={() => {
            props.history.push("/logout");
          }}
        >
          <ListItemIcon>
            <ExitToAppIcon className={classes.drawerOption} />
          </ListItemIcon>
          <ListItemText
            className={classes.drawerOption}
            primary={"CERRAR SESIÓN"}
          />
        </ListItem>
      </List>
    </div>
  );

  return (
    <div className={classes.root}>
      <CssBaseline />
      {(typeof props.showHeader === "undefined" || props.showHeader) && (
        <AppBar position="fixed" className={classes.appBar}>
          <Toolbar>
            <IconButton
              color="inherit"
              aria-label="open drawer"
              edge="start"
              onClick={handleDrawerToggle}
              className={classes.menuButton}
            >
              <MenuIcon />
            </IconButton>
            <Typography variant="h5" noWrap>
              {props.title}
            </Typography>
          </Toolbar>
        </AppBar>
      )}

      <nav className={classes.drawer} aria-label="mailbox folders">
        {/* The implementation can be swapped with js to avoid SEO duplication of links. */}
        <Hidden smUp implementation="css">
          <Drawer
            container={container}
            variant="temporary"
            anchor={theme.direction === "rtl" ? "right" : "left"}
            open={mobileOpen}
            onClose={handleDrawerToggle}
            classes={{
              paper: classes.drawerPaper,
            }}
            ModalProps={{
              keepMounted: true, // Better open performance on mobile.
            }}
          >
            {drawer}
          </Drawer>
        </Hidden>
        <Hidden xsDown implementation="css">
          <Drawer
            classes={{
              paper: classes.drawerPaper,
            }}
            variant="permanent"
            open
          >
            {drawer}
          </Drawer>
        </Hidden>
      </nav>
      <main className={classes.content}>
        {(typeof props.showHeader === "undefined" || props.showHeader) && (
          <div className={classes.toolbarBody} />
        )}
        {props.children}
      </main>
    </div>
  );
}

Layout.propTypes = {
  /**
   * Injected by the documentation to work in an iframe.
   * You won't need it on your project.
   */
  container: PropTypes.instanceOf(
    typeof Element === "undefined" ? Object : Element
  ),
};

export default withRouter(Layout);
