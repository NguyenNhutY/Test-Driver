import React from 'react';
import './../styles/cta.css';
import { Link } from 'react-router-dom'; // Import Link từ React Router


const CTA = () => (

  <div className="cta">
    <div className="cta-content">
      <h3>Thi Thử Bằng Lái Xe Online</h3>
    </div>
    <div className="cta-btn">
            <Link to="/thi-ly-thuyet">
                <button type="button">Bắt Đầu</button>
            </Link>
     </div>
  </div>
);

export default CTA;
