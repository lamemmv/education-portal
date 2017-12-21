import { h, Component } from 'preact';
import { connect } from 'preact-redux';
import { bindActionCreators } from 'redux';
import { addNotification } from './notification.actions';
import NotificationSystem from 'react-notification-system';

class NotificationContainer extends Component {

    constructor(props) {
        super(props);
    }

    componentWillReceiveProps(newProps) {
        const { message, level } = newProps.notification;
        this.notificationSystem.addNotification({
            message,
            level
        });
    }

    render() {
        return (
            <NotificationSystem ref={input => {
                this.notificationSystem = input;
            }} />
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