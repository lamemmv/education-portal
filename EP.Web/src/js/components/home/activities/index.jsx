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
            // <Carousel
            //     showThumbs={false}
            //     autoPlay={true}
            //     showStatus={false}
            //     transitionTime={1000}
            //     interval={5000}
            //     axis="vertical">
            //     <div>
            //         <img src={activity1}
            //             ref={(node) => {
            //                 if (node) {
            //                     node.style.setProperty("width", "100%", "important");
            //                 }
            //             }} />
            //     </div>
            //     <div>
            //         <img src={activity2} ref={(node) => {
            //             if (node) {
            //                 node.style.setProperty("width", "100%", "important");
            //             }
            //         }} />
            //     </div>
            //     <div>
            //         <img src={activity3} ref={(node) => {
            //             if (node) {
            //                 node.style.setProperty("width", "100%", "important");
            //             }
            //         }} />
            //     </div>
            //     <div>
            //         <img src={activity4} ref={(node) => {
            //             if (node) {
            //                 node.style.setProperty("width", "100%", "important");
            //             }
            //         }} />
            //     </div>
            //     <div>
            //         <img src={activity5} ref={(node) => {
            //             if (node) {
            //                 node.style.setProperty("width", "100%", "important");
            //             }
            //         }} />
            //     </div>
            //     <div>
            //         <img src={activity6} ref={(node) => {
            //             if (node) {
            //                 node.style.setProperty("width", "100%", "important");
            //             }
            //         }} />
            //     </div>
            //     <div>
            //         <img src={activity7} ref={(node) => {
            //             if (node) {
            //                 node.style.setProperty("width", "100%", "important");
            //             }
            //         }} />
            //     </div>
            //     <div>
            //         <img src={activity8} ref={(node) => {
            //             if (node) {
            //                 node.style.setProperty("width", "100%", "important");
            //             }
            //         }} />
            //     </div>
            //     <div>
            //         <img src={activity9} ref={(node) => {
            //             if (node) {
            //                 node.style.setProperty("width", "100%", "important");
            //             }
            //         }} />
            //     </div>
            // </Carousel>
            <div class="container marketing">
                <div class="row">
                    <div class="col-lg-4">
                        <img class="rounded-circle" src="data:image/gif;base64,R0lGODlhAQABAIAAAHd3dwAAACH5BAAAAAAALAAAAAABAAEAAAICRAEAOw==" alt="Generic placeholder image" width="140" height="140" />
                        <h2>Heading</h2>
                        <p>Donec sed odio dui. Etiam porta sem malesuada magna mollis euismod. Nullam id dolor id nibh ultricies vehicula ut id elit. Morbi leo risus, porta ac consectetur ac, vestibulum at eros. Praesent commodo cursus magna.</p>
                        <p><a class="btn btn-secondary" href="#" role="button">View details »</a></p>
                    </div>
                    <div class="col-lg-4">
                        <img class="rounded-circle" src="data:image/gif;base64,R0lGODlhAQABAIAAAHd3dwAAACH5BAAAAAAALAAAAAABAAEAAAICRAEAOw==" alt="Generic placeholder image" width="140" height="140" />
                        <h2>Heading</h2>
                        <p>Duis mollis, est non commodo luctus, nisi erat porttitor ligula, eget lacinia odio sem nec elit. Cras mattis consectetur purus sit amet fermentum. Fusce dapibus, tellus ac cursus commodo, tortor mauris condimentum nibh.</p>
                        <p><a class="btn btn-secondary" href="#" role="button">View details »</a></p>
                    </div>
                    <div class="col-lg-4">
                        <img class="rounded-circle" src="data:image/gif;base64,R0lGODlhAQABAIAAAHd3dwAAACH5BAAAAAAALAAAAAABAAEAAAICRAEAOw==" alt="Generic placeholder image" width="140" height="140" />
                        <h2>Heading</h2>
                        <p>Donec sed odio dui. Cras justo odio, dapibus ac facilisis in, egestas eget quam. Vestibulum id ligula porta felis euismod semper. Fusce dapibus, tellus ac cursus commodo, tortor mauris condimentum nibh, ut fermentum massa justo sit amet risus.</p>
                        <p><a class="btn btn-secondary" href="#" role="button">View details »</a></p>
                    </div>
                </div>
                <hr class="featurette-divider" />
                <div class="row featurette">
                    <div class="col-md-7">
                        <h2 class="featurette-heading">First featurette heading. <span class="text-muted">It'll blow your mind.</span></h2>
                        <p class="lead">Donec ullamcorper nulla non metus auctor fringilla. Vestibulum id ligula porta felis euismod semper. Praesent commodo cursus magna, vel scelerisque nisl consectetur. Fusce dapibus, tellus ac cursus commodo.</p>
                    </div>
                    <div class="col-md-5">
                        <img class="featurette-image img-fluid mx-auto"
                            data-src="holder.js/500x500/auto"
                            alt="500x500"
                            src={activity1}
                            data-holder-rendered="true" />
                    </div>
                </div>
                <hr class="featurette-divider" />
                <div class="row featurette">
                    <div class="col-md-7 order-md-2">
                        <h2 class="featurette-heading">Oh yeah, it's that good. <span class="text-muted">See for yourself.</span></h2>
                        <p class="lead">Donec ullamcorper nulla non metus auctor fringilla. Vestibulum id ligula porta felis euismod semper. Praesent commodo cursus magna, vel scelerisque nisl consectetur. Fusce dapibus, tellus ac cursus commodo.</p>
                    </div>
                    <div class="col-md-5 order-md-1">
                        <img class="featurette-image img-fluid mx-auto"
                            data-src="holder.js/500x500/auto"
                            alt="500x500"
                            src={activity2}
                            data-holder-rendered="true" />
                    </div>
                </div>
                <hr class="featurette-divider" />
                <div class="row featurette">
                    <div class="col-md-7">
                        <h2 class="featurette-heading">And lastly, this one. <span class="text-muted">Checkmate.</span></h2>
                        <p class="lead">Donec ullamcorper nulla non metus auctor fringilla. Vestibulum id ligula porta felis euismod semper. Praesent commodo cursus magna, vel scelerisque nisl consectetur. Fusce dapibus, tellus ac cursus commodo.</p>
                    </div>
                    <div class="col-md-5">
                        <img class="featurette-image img-fluid mx-auto"
                            data-src="holder.js/500x500/auto"
                            alt="500x500"
                            src={activity3}
                            data-holder-rendered="true" />
                    </div>
                </div>
                <hr class="featurette-divider" />
            </div>
        );
    }
}

export default EPActivities;