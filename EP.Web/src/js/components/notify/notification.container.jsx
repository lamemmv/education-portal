import { h, Component } from 'preact';
import { connect } from 'preact-redux';
import { bindActionCreators } from 'redux';
import { addNotification } from './notification.actions';
//import { MDCSnackbar, MDCSnackbarFoundation } from '@material/snackbar';

class NotificationContainer extends Component {

    constructor(props) {
        super(props);
    }

    componentWillReceiveProps(newProps) {
        const { message, level, title } = newProps.notification;
        const snackbar = new MDCSnackbar(this.notificationSystem);
        let snackBarInfo = {
            message: message,
            actionText: 'Close',
            actionHandler: function () {
              console.log('my cool function');
            }
        };
        snackbar.show(snackBarInfo);
    }

    render() {
        const { message, level, title } = this.props.notification;
        return (
            <div
                ref={input => {
                    this.notificationSystem = input;
                }}
                class="mdc-snackbar"
                aria-live="assertive"
                aria-atomic="true"
                aria-hidden="true">
                <div class="mdc-snackbar__text">{message}</div>
                <div class="mdc-snackbar__action-wrapper">
                    <button type="button" class="mdc-snackbar__action-button">Close</button>
                </div>
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