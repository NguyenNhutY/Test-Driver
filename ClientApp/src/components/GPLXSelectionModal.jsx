// Tạo một component mới tên là GPLXSelectionModal
import React, { Component } from 'react';
import '../styles/NavMenu.css'
class GPLXSelectionModal extends Component {
    render() {
        const { showModal, licenseTypes, selectedLicenseType, submitForm, closeModal } = this.props;

        return (
            showModal && (
                <div className="fixed inset-0 z-50 flex items-center justify-center overflow-auto bg-gray-800 bg-opacity-75">
                    <div className="ant-modal" style={{ width: '520px', transformOrigin: '506px -69px' }}>
                        <div className="ant-modal-content bg-white rounded-lg shadow-xl">
                            {/* Header */}
                            <div className="ant-modal-header bg-gray-200 rounded-t-lg p-4">
                                <div className="ant-modal-title" id="rcDialogTitle0">Chọn hạng GPLX</div>
                            </div>
                            {/* Body */}
                            <div className="ant-modal-body p-4">
                                {licenseTypes.map(licenseType => (
                                    <button
                                        key={licenseType.idHang}
                                        type="button"
                                        className={`w-24 h-12 m-1 ${parseInt(licenseType.idHang) === parseInt(selectedLicenseType.idHang) ? 'bg-gray-500 text-white' : 'bg-gray-100'} hover:bg-gray-500 hover:text-white`}
                                        onClick={() => this.setState({ selectedLicenseType: licenseType })}
                                    >
                                        <span>{licenseType.thongtin}</span>
                                    </button>
                                ))}
                            </div>
                            {/* Footer */}
                            <div className="bg-gray-200 rounded-b-lg p-4 flex justify-around">
                                <button type="button" onClick={submitForm}><span>OK</span></button>
                                <button type="button" onClick={closeModal}><span>Cancel</span></button>
                            </div>
                        </div>
                    </div>
                </div>
            )
        );
    }
}

export default GPLXSelectionModal;
