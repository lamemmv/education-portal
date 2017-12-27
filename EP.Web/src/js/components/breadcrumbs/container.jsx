import {
    connect
} from 'preact-redux';

import {
    bindActionCreators
} from 'redux';

import * as BreadcrumbsActions from './actions';

import Breadcrumbs from './index';

const mapStateToProps = (state) => {
    return {
        breadcrumbs: state.breadcrumbs
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        actions: bindActionCreators(BreadcrumbsActions, dispatch)
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Breadcrumbs);