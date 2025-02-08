using Core.Entities;
using Core.IRepository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class VirtualAppointmentRepository : IVirtualAppointmentRepository
    {
        private readonly ApplicationDBContext _context;

        public VirtualAppointmentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<VirtualAppointment>> GetVirtualAppointmentListAsync() => await _context.VirtualAppointment.ToListAsync();

        public async Task<VirtualAppointment> GetVirtualAppointmentAsync(int id) => await _context.VirtualAppointment.FirstOrDefaultAsync(x=>x.Id == id);

        public async Task<bool> DeleteVirtualAppointmentAsync(int id)
        {
            var result = await GetVirtualAppointmentAsync(id);

            if (result != null) { 
                _context.VirtualAppointment.Remove(result);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<VirtualAppointment> SaveVirtualAppointment(VirtualAppointment virtualAppointment)
        {
            if (virtualAppointment == null)
            {
                throw new ArgumentNullException(nameof(virtualAppointment), "Virtual Appointment cannot be null.");
            }

            if (virtualAppointment.Id > 0)
            {
                // Update existing VirtualAppointment
                var existingAppointment = await _context.VirtualAppointment
                    .FirstOrDefaultAsync(a => a.Id == virtualAppointment.Id);

                if (existingAppointment != null)
                {
                    // Update the fields you need to update
                    existingAppointment.RegisterDate = virtualAppointment.RegisterDate;
                    existingAppointment.RegisterTime = virtualAppointment.RegisterTime;
                    existingAppointment.Message = virtualAppointment.Message;
                    existingAppointment.FirstName = virtualAppointment.FirstName;
                    existingAppointment.LastName = virtualAppointment.LastName;
                    existingAppointment.CompanyName = virtualAppointment.CompanyName;
                    existingAppointment.CategoryId = virtualAppointment.CategoryId;
                    // Add more fields to update as required

                    _context.VirtualAppointment.Update(existingAppointment);
                }
                else
                {
                    // Appointment with this ID does not exist. You might want to throw an exception or handle this case.
                    throw new KeyNotFoundException("Virtual Appointment not found.");
                }
            }
            else
            {
                // Add new VirtualAppointment
                await _context.VirtualAppointment.AddAsync(virtualAppointment);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            return virtualAppointment;
        }

    }
}
