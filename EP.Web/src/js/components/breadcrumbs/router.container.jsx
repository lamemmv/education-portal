import { h, Component } from 'preact';
import { connect } from 'preact-redux';
import { withRouter } from 'react-router-dom';
import * as BreadcrumbsAction from './actions';

class RouterContainer extends Component {

  constructor(props, context) {
    super(props, context);
    this.handleRouteChange = this.handleRouteChange.bind(this);
  }

  componentWillMount() {
    const store = this.context.store;
    this.unlisten = this.props.history.listen((location, action) => {
      this.handleRouteChange(location, store);
    });

  }

  handleRouteChange = (location, store) => {
    let pathname = location.pathname;
    switch (pathname) {
      case '/files':
        store.dispatch(BreadcrumbsAction.gotoFiles([
          { id: 'home', path: '/' },
          { id: 'files', path: pathname }]));
        break;
      case '/news':
        store.dispatch(BreadcrumbsAction.gotoHomeNews([
          { id: 'home', path: '/' },
          { id: 'news', path: pathname }]));
        break;
      default:
        store.dispatch(BreadcrumbsAction.gotoHome([{ id: 'home', path: pathname }]));
        break;
    }
  }

  componentDidUpdate(prevProps) {
    if (this.props.location !== prevProps.location) {
      console.log(this.props.location);
    }
  }

  componentWillUnmount() {
    this.unlisten();
  }

  render() {
    return (
      <div>{this.props.children}</div>
    );
  }
}

export default withRouter(RouterContainer);