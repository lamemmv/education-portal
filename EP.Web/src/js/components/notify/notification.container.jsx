import { h, Component } from 'preact';
import { connect } from 'preact-redux';
import { bindActionCreators } from 'redux';
let classNames = require('classnames');
import toastr from 'toastr';
import { addNotification } from './notification.actions';

class NotificationContainer extends Component {

    componentWillReceiveProps(nextProps) {
        if (nextProps.notification.message) {
            switch (nextProps.notification.level) {
                case 'success':
                    toastr.success(nextProps.notification.message);
                    break;
                case 'error':
                    toastr.error(nextProps.notification.message);
                    break;
                default:
                    toastr.info(nextProps.notification.message);
                    break;
            }
        }
    }
    render() {
        const { message, level, title } = this.props.notification;
        return (
            <div>
                {/* {
                    message ? (<div id='ep-notification'
                        className={classNames('alert', {
                            'alert-success': level == 'success',
                            'alert-danger': level == 'error',
                            'alert-info': level == 'info'
                        })}
                        role="alert">
                        {message}
                    </div>) : null
                } */}
            </div>
        );
    }
}

function mapStateToProps(state) {
    return {
        notification: state.notification
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators({
            addNotification
        }, dispatch)
    };
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(NotificationContainer);