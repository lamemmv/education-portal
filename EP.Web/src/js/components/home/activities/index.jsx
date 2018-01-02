import { h, Component } from 'preact';

import { Carousel } from 'react-responsive-carousel';

import * as styles from './styles.css';
import activity1 from '../../../../assets/images/activity-1.jpg';
import activity2 from '../../../../assets/images/activity-2.jpg';
import activity3 from '../../../../assets/images/activity-3.jpg';
import activity4 from '../../../../assets/images/activity-4.jpg';
import activity5 from '../../../../assets/images/activity-5.jpg';
import activity6 from '../../../../assets/images/activity-6.jpg';
import activity7 from '../../../../assets/images/activity-7.jpg';
import activity8 from '../../../../assets/images/activity-8.jpg';
import activity9 from '../../../../assets/images/activity-9.jpg';

const width = {
    width: {
        value: '100%',
        important: 'true'
    }
}
class EPActivities extends Component {
    render() {
        return (
            <Carousel
                showThumbs={false}
                autoPlay={true}
                showStatus={false}
                transitionTime={1000}
                interval={5000}
                axis="vertical">
                <div>
                    <img src={activity1}
                        ref={(node) => {
                            if (node) {
                                node.style.setProperty("width", "100%", "important");
                            }
                        }} />
                </div>
                <div>
                    <img src={activity2} ref={(node) => {
                        if (node) {
                            node.style.setProperty("width", "100%", "important");
                        }
                    }} />
                </div>
                <div>
                    <img src={activity3} ref={(node) => {
                        if (node) {
                            node.style.setProperty("width", "100%", "important");
                        }
                    }} />
                </div>
                <div>
                    <img src={activity4} ref={(node) => {
                        if (node) {
                            node.style.setProperty("width", "100%", "important");
                        }
                    }} />
                </div>
                <div>
                    <img src={activity5} ref={(node) => {
                        if (node) {
                            node.style.setProperty("width", "100%", "important");
                        }
                    }} />
                </div>
                <div>
                    <img src={activity6} ref={(node) => {
                        if (node) {
                            node.style.setProperty("width", "100%", "important");
                        }
                    }} />
                </div>
                <div>
                    <img src={activity7} ref={(node) => {
                        if (node) {
                            node.style.setProperty("width", "100%", "important");
                        }
                    }} />
                </div>
                <div>
                    <img src={activity8} ref={(node) => {
                        if (node) {
                            node.style.setProperty("width", "100%", "important");
                        }
                    }} />
                </div>
                <div>
                    <img src={activity9} ref={(node) => {
                        if (node) {
                            node.style.setProperty("width", "100%", "important");
                        }
                    }} />
                </div>
            </Carousel>
        );
    }
}

export default EPActivities;