using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AdminDashboard.Server.Entities;
using AdminDashboard.Server.Helpers;
using AdminDashboard.Server.ViewModals;
using AdminDashboard.Server.ViewModels;
using Newtonsoft.Json;

namespace AdminDashboard.Server.Services
{
    public class VirtualAppointmentServices
    {
        private readonly HttpClient _httpClient;

        public VirtualAppointmentServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<VirtualAppointmentVM>> GetVirtualAppointmentList()
        {
            var response = await _httpClient.GetAsync($"{@AppStatic.ApiUrl}/api/VirtualAppointment/VirtualAppoimentList");

            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var responseContent = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into a list of VirtualAppointment objects
                var virtualAppointments = JsonConvert.DeserializeObject<IReadOnlyList<VirtualAppointmentVM>>(responseContent);


                // Return the list of virtual appointments
                return virtualAppointments;
            }
            else
            {
                // Handle unsuccessful response, you can log the error or throw an exception
                // For now, you can return an empty list or handle the error as needed
                return new List<VirtualAppointmentVM>();
            }

        }

        public async Task<VirtualAppointmentVM> GetVirtualAppointmentById(int id)
        {
            var response = await _httpClient.GetAsync($"{@AppStatic.ApiUrl}/api/VirtualAppointment/VirtualAppoiment/AppointmentById/{id}");

            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var responseContent = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into a list of VirtualAppointment objects
                var virtualAppointments = JsonConvert.DeserializeObject<VirtualAppointmentVM>(responseContent);


                // Return the list of virtual appointments
                return virtualAppointments;
            }
            else
            {
                // Handle unsuccessful response, you can log the error or throw an exception
                // For now, you can return an empty list or handle the error as needed
                return new VirtualAppointmentVM();
            }

        }

            public async Task<VirtualAppointmentVM> SaveAndUpdateData(VirtualAppointmentVM appointmentVM)
            {

                // If an appointment ID exists, send an update request
                var response = await _httpClient.PostAsJsonAsync($"{@AppStatic.ApiUrl}/api/VirtualAppointment/SaveVirtualAppointment", appointmentVM);

                if (response.IsSuccessStatusCode)
                {
                    // If the update is successful, deserialize and return the updated appointment
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var updatedAppointment = JsonConvert.DeserializeObject<AppointmentResponse>(responseContent);

                    var modifiedDT = new VirtualAppointmentVM
                    {
                        CompanyName=updatedAppointment.Data.CompanyName,
                        FirstName=updatedAppointment.Data.FirstName,
                        LastName=updatedAppointment.Data.LastName,
                        Message=updatedAppointment.Data.Message,
                        RegisterDate=updatedAppointment.Data.RegisterDate,
                        RegisterTime=updatedAppointment.Data.RegisterTime,
                        Status=updatedAppointment.Data.Status,
                        Id=updatedAppointment.Data.Id,
                        CategoryId=updatedAppointment.Data.CategoryId,
                        EmailId=updatedAppointment.Data.EmailId,
                    };


                    return modifiedDT;
                }
                else
                {
                    // Handle unsuccessful response (e.g., log error, throw exception)
                    return null;
                }

            }


    }
}
