using Core.Entities;
using Core.ViewModals;

namespace Core.IRepository
{
    public interface IVirtualAppointmentRepository
    {
        Task<IReadOnlyList<VirtualAppointmentVM>> GetVirtualAppointmentListAsync();

        Task<VirtualAppointmentVM> GetVirtualAppointmentAsync(int id);

        Task<VirtualAppointment> SaveVirtualAppointment(VirtualAppointment virtualAppointment);

        Task<bool> DeleteVirtualAppointmentAsync(int id);
    }
}
