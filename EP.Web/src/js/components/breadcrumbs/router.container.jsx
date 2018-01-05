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
    this.handleRouteChange(this.props.location, store);
  }

  handleRouteChange = (location, store) => {
    let pathname = location.pathname;
    switch (true) {
      case pathname == '/files':
        store.dispatch(BreadcrumbsAction.gotoFiles());
        break;
      case /\/files\/(?!create|list).*/.test(pathname):
        store.dispatch(BreadcrumbsAction.gotoFilePreview(pathname));
        break;
      case pathname == '/news':
        store.dispatch(BreadcrumbsAction.gotoHomeNews());
        break;
      case pathname == '/news/create':
        store.dispatch(BreadcrumbsAction.gotoNewsCreate());
        break;
      case /\/news\/(?!create|list).*/.test(pathname):
        store.dispatch(BreadcrumbsAction.gotoNewsDetail(pathname));
        break;
      default:
        store.dispatch(BreadcrumbsAction.gotoHome());
        break;
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