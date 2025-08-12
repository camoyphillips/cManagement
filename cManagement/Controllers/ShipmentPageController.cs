using cManagement.Interfaces;
using cManagement.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace cManagement.Controllers
{
    /// <summary>
    /// Handles web requests for Shipment-related operations, including CRUD and related listings.
    /// </summary>
    public class ShipmentPageController(IShipmentService shipmentService) : Controller
    {

        /// <summary>
        /// Displays a list of all shipments.
        /// </summary>
        /// <returns>The view displaying the list of shipments.</returns>
        public async Task<IActionResult> List()
        {
            var shipments = await shipmentService.ListShipments();
            return View(shipments);
        }

        /// <summary>
        /// Displays the detailed information for a single shipment.
        /// </summary>
        /// <param name="id">The unique identifier of the shipment.</param>
        /// <returns>The view displaying shipment details, or redirects to list if not found.</returns>
        public async Task<IActionResult> Details(int id)
        {
            var shipment = await shipmentService.FindShipment(id);
            if (shipment == null)
            {
                TempData["ErrorMessage"] = "Shipment not found.";
                return RedirectToAction(nameof(List));
            }
            return View(shipment);
        }

        /// <summary>
        /// Displays the form for creating a new shipment.
        /// </summary>
        /// <returns>The view with the new shipment form.</returns>
        public IActionResult New()
        {
            // Example of populating a dropdown for TruckId if needed:
            // var trucks = await _truckService.ListTrucks();
            // ViewBag.Trucks = new SelectList;
            return View(new ShipmentDto()); 
        }

        /// <summary>
        /// Handles the submission of the new shipment form.
        /// </summary>
        /// <param name="shipmentDto">The DTO containing the new shipment data from the form.</param>
        /// <returns>Redirects to details on success, or redisplays the form with errors.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ShipmentDto shipmentDto)
        {
            if (!ModelState.IsValid)
            {
                // Re-populate ViewBag.Trucks if you're using a dropdown
                // var trucks = await _truckService.ListTrucks();
                // ViewBag.Trucks = new SelectList;
                return View("New", shipmentDto);
            }

            var response = await shipmentService.AddShipment(shipmentDto);

            if (response.Status == Models.ServiceResponse.ServiceStatus.Created)
            {
                TempData["SuccessMessage"] = "Shipment added successfully!";
                return RedirectToAction(nameof(Details), new { id = response.CreatedId });
            }
            else
            {
                TempData["ErrorMessage"] = "Error adding shipment.";
                // var trucks = await _truckService.ListTrucks();
                // ViewBag.Trucks = new SelectList;
                return View("New", shipmentDto);
            }
        }

        /// <summary>
        /// Displays the form for editing an existing shipment.
        /// </summary>
        /// <param name="id">The unique identifier of the shipment to edit.</param>
        /// <returns>The view with the edit shipment form, or redirects to list if not found.</returns>
        public async Task<IActionResult> Edit(int id)
        {
            var shipment = await shipmentService.FindShipment(id);
            if (shipment == null)
            {
                TempData["ErrorMessage"] = "Shipment not found for editing.";
                return RedirectToAction(nameof(List));
            }

            // var trucks = await _truckService.ListTrucks();
            // ViewBag.Trucks = new SelectList;
            return View(shipment);
        }

        /// <summary>
        /// Handles the submission of the edited shipment form.
        /// </summary>
        /// <param name="id">The unique identifier of the shipment (from route).</param>
        /// <param name="shipmentDto">The DTO containing the updated shipment data from the form.</param>
        /// <returns>Redirects to details on success, or redisplays the form with errors.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, ShipmentDto shipmentDto)
        {
            if (id != shipmentDto.ShipmentId)
            {
                TempData["ErrorMessage"] = "Shipment ID mismatch.";
                return RedirectToAction(nameof(List));
            }

            if (!ModelState.IsValid)
            {
                // var trucks = await _truckService.ListTrucks();
                // ViewBag.Trucks = new SelectList;
                return View("Edit", shipmentDto);
            }

            var response = await shipmentService.UpdateShipment(shipmentDto);

            if (response.Status == Models.ServiceResponse.ServiceStatus.Updated)
            {
                TempData["SuccessMessage"] = "Shipment updated successfully!";
                return RedirectToAction(nameof(Details), new { id = shipmentDto.ShipmentId });
            }
            else if (response.Status == Models.ServiceResponse.ServiceStatus.NotFound)
            {
                TempData["ErrorMessage"] = "Shipment not found for update.";
                return RedirectToAction(nameof(List));
            }
            else
            {
                TempData["ErrorMessage"] = "Error updating shipment.";
                // var trucks = await _truckService.ListTrucks();
                // ViewBag.Trucks = new SelectList;
                return View("Edit", shipmentDto);
            }
        }


        /// <summary>
        /// Displays a confirmation page before deleting a shipment.
        /// </summary>
        /// <param name="id">The unique identifier of the shipment to delete.</param>
        /// <returns>The view displaying the delete confirmation, or redirects to list if not found.</returns>
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var shipment = await shipmentService.FindShipment(id);
            if (shipment == null)
            {
                TempData["ErrorMessage"] = "Shipment not found for deletion.";
                return RedirectToAction(nameof(List));
            }
            return View(shipment);
        }

        /// <summary>
        /// Handles the actual deletion of a shipment after confirmation.
        /// </summary>
        /// <param name="id">The unique identifier of the shipment to delete.</param>
        /// <returns>Redirects to the shipment list.</returns>
        [HttpPost, ActionName("Delete")] 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await shipmentService.DeleteShipment(id);

            if (response.Status == Models.ServiceResponse.ServiceStatus.Deleted)
            {
                TempData["SuccessMessage"] = "Shipment deleted successfully!";
            }
            else if (response.Status == Models.ServiceResponse.ServiceStatus.NotFound)
            {
                TempData["ErrorMessage"] = "Shipment not found for deletion.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error deleting shipment.";
            }

            return RedirectToAction(nameof(List));
        }


        /// <summary>
        /// Displays a list of shipments assigned to a specific truck.
        /// </summary>
        /// <param name="truckId">The unique identifier of the truck.</param>
        /// <returns>The view displaying shipments for the specified truck.</returns>
        public async Task<IActionResult> ShipmentsForTruck(int truckId)
        {
            var shipments = await shipmentService.ListShipmentsForTruck(truckId);
            ViewData["TruckId"] = truckId; 
            return View(shipments); 
        }

        /// <summary>
        /// Displays a list of shipments associated with a specific driver.
        /// </summary>
        /// <param name="driverId">The unique identifier of the driver.</param>
        /// <returns>The view displaying shipments for the specified driver.</returns>
        public async Task<IActionResult> ShipmentsForDriver(int driverId)
        {
            var shipments = await shipmentService.ListShipmentsForDriver(driverId);
            ViewData["DriverId"] = driverId; 
            return View(shipments);
        }
    }
}